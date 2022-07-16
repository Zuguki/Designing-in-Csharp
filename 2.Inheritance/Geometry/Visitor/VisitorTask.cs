
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Visitor
{

    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract object Accept(IVisitor visitor);

        public abstract bool ContainsPoint(Vector3 point);

        public abstract RectangularCuboid GetBoundingBox();
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override object Accept(IVisitor visitor) => visitor.Visit(this);

        public override bool ContainsPoint(Vector3 point)
        {
            var vector = point - Position;
            var length2 = vector.GetLength2();
            return length2 <= Radius * Radius;
        }

        public override RectangularCuboid GetBoundingBox() => 
            new RectangularCuboid(Position, Radius * 2, Radius * 2, Radius * 2);
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public Vector3 MinPoint => new Vector3(
            Position.X - SizeX / 2,
            Position.Y - SizeY / 2,
            Position.Z - SizeZ / 2);

        public Vector3 MaxPoint => new Vector3(
            Position.X + SizeX / 2,
            Position.Y + SizeY / 2,
            Position.Z + SizeZ / 2);

        public override object Accept(IVisitor visitor) => visitor.Visit(this);

        public override bool ContainsPoint(Vector3 point) =>
            point >= MinPoint && point <= MaxPoint;

        public override RectangularCuboid GetBoundingBox() => this;
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override object Accept(IVisitor visitor) => visitor.Visit(this);

        public override bool ContainsPoint(Vector3 point)
        {
            var vectorX = point.X - Position.X;
            var vectorY = point.Y - Position.Y;
            var length2 = vectorX * vectorX + vectorY * vectorY;
            var minZ = Position.Z - SizeZ / 2;
            var maxZ = minZ + SizeZ;

            return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
        }

        public override RectangularCuboid GetBoundingBox() =>
            new RectangularCuboid(Position, Radius * 2, Radius * 2, SizeZ);
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override object Accept(IVisitor visitor) => visitor.Visit(this);

        public override bool ContainsPoint(Vector3 point) => Parts.Any(body => body.ContainsPoint(point));
        
        public override RectangularCuboid GetBoundingBox()
        {
            (double min, double max) x = (0, 0);
            (double min, double max) y = (0, 0);
            (double min, double max) z = (0, 0);

            var boxes = Parts.Select(box => box.GetBoundingBox()).ToList();
            x.min = boxes.Min(box => box.MinPoint.X);
            y.min = boxes.Min(box => box.MinPoint.Y);
            z.min = boxes.Min(box => box.MinPoint.Z);

            x.max = boxes.Max(box => box.MaxPoint.X);
            y.max = boxes.Max(box => box.MaxPoint.Y);
            z.max = boxes.Max(box => box.MaxPoint.Z);
            
            return new RectangularCuboid(new Vector3((x.max + x.min) / 2, (y.max + y.min) / 2, (z.max + z.min) / 2),
                x.max - x.min, y.max - y.min, z.max - z.min);
        }
    }

    public class BoundingBoxVisitor : IVisitor
    {
        public RectangularCuboid Visit(Ball ball) => ball.GetBoundingBox();

        public RectangularCuboid Visit(Cylinder cylinder) => cylinder.GetBoundingBox();

        public RectangularCuboid Visit(RectangularCuboid rectangularCuboid) => rectangularCuboid.GetBoundingBox();

        public RectangularCuboid Visit(CompoundBody compoundBody) => compoundBody.GetBoundingBox();
    }

    public class BoxifyVisitor : IVisitor
    {
        public RectangularCuboid Visit(Ball ball)
        {
            throw new System.NotImplementedException();
        }

        public RectangularCuboid Visit(Cylinder cylinder)
        {
            throw new System.NotImplementedException();
        }

        public RectangularCuboid Visit(RectangularCuboid rectangularCuboid)
        {
            throw new System.NotImplementedException();
        }

        public RectangularCuboid Visit(CompoundBody compoundBody)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IVisitor
    {
        RectangularCuboid Visit(Ball ball);
        RectangularCuboid Visit(Cylinder cylinder);
        RectangularCuboid Visit(RectangularCuboid rectangularCuboid);
        RectangularCuboid Visit(CompoundBody compoundBody);
    }
}