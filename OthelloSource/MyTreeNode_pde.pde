import java.util.List;

public class MyTreeNode<T>{
    public Configuracion data;
    public List<MyTreeNode> children = new ArrayList<MyTreeNode>();
    public MyTreeNode parent = null;

    public MyTreeNode(Configuracion data) {
        this.data = data;
    }

    public void addChild(MyTreeNode child) {
        child.setParent(this);
        this.children.add(child);
    }

    public void addChild(Configuracion data) {
        MyTreeNode<Configuracion> newChild = new MyTreeNode(data);
        newChild.setParent(this);
        children.add(newChild);
    }

    public void addChildren(List<MyTreeNode> children) {
        for(MyTreeNode t : children) {
            t.setParent(this);
        }
        this.children.addAll(children);
    }

    public List<MyTreeNode> getChildren() {
        return children;
    }

    public Configuracion getData() {
        return data;
    }

    public void setData(Configuracion data) {
        this.data = data;
    }

    private void setParent(MyTreeNode parent) {
        this.parent = parent;
    }

    public MyTreeNode getParent() {
        return parent;
    }
}
